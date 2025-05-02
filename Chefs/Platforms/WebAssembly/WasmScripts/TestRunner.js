class SampleRunner {

	static async init() {

		if (!SampleRunner._getCurrentPage) {
			const chefsExports = await Module.getAssemblyExports("Chefs");

			SampleRunner._getCurrentPage = chefsExports.Chefs.App.GetCurrentPage;
		}
	}

	static async GetCurrentPage(unused) {
		await SampleRunner.init();
		return SampleRunner._getCurrentPage();
	}
}

globalThis.SampleRunner = SampleRunner;
